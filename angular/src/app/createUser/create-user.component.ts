import { Component } from '@angular/core'
import { IRegisterUser } from 'src/interfaces/registerUser'
import { IUser } from 'src/interfaces/user';
import { UserService } from 'src/services/user.service';

@Component({
    selector: 'create-user',
    templateUrl: 'create-user.component.html',
    styleUrls: ['create-user.component.css'],
})

export class CreateUserComponent {
    createUser:IRegisterUser = {
        id: 0,
        username: "",
        password: "password",
        role: "",
        teamId: 0,
        firstName: "",
        lastName: "",
        calibrationPosition: 0,
    }

    editUser:IUser = {
        userId: 0,
        username: "",
        role: "",
        teamId: 0,
        firstName: "",
        lastName: "",
        isActive: true,
        calibrationPosition: 0,
    }
    
    roles:any[] =
    [
        {
            id: 1,
            name: "Admin",
        },
        {
            id: 2,
            name: "Participant",
        }
    ];

    teams:any;
    users:any;
    userIssue:boolean = false;
    addUser:boolean = false;

    constructor(private userService:UserService) {
        this.userService.getTeams().subscribe(data => {
            this.teams = data;
        });

        this.userService.getAllUsers().subscribe(data => {
            this.users = data;
        })
    }

    FindTeam(user:IUser) {
        let team = this.teams.find((x:any) => x.id === user.teamId);
        return (team.name);
    }
    
    EditCheck(user:IUser) {
        return (user.userId === this.editUser.userId);
    }

    AddNewUser() {
        this.addUser = true;
    }

    CreateNewUser(user:IRegisterUser) {
        // this needs to be updated as a created user is not getting their calibration position
        user.calibrationPosition = Math.max(...this.users.map((x:IUser) => x.calibrationPosition)) +1;

        this.userService.createUser(user).subscribe(data => {
            this.users.push(data);
            this.ResetCreateUser();

            this.addUser = false;
        });
    }

    ResetCreateUser() {
        this.createUser = {
            id: 0,
            username: "",
            password: "password",
            role: "",
            teamId: 0,
            firstName: "",
            lastName: "",
            calibrationPosition: 0,
        };
    }

    SwitchUserIsActive(user:IUser) {
        this.userService.switchActive(user.userId).subscribe((data) => {
            if(!data) {
                console.log("Didn't switch");
            }
            else {
                this.users.find((x:any) => x.userId === user.userId).isActive = !this.users.find((x:any) => x.userId === user.userId).isActive;
            }
        });
    }

    EditUser(user:IUser) {
        this.editUser.userId = user.userId;
        this.editUser.username = user.username;
        this.editUser.role = user.role;
        this.editUser.teamId = user.teamId;
        this.editUser.firstName = user.firstName;
        this.editUser.lastName = user.lastName;
    }

    SaveUserEdit(user:IUser) {
        this.userService.updateUser(user).subscribe((data) => {
            if (!data) {
                console.log("Unable to update");
            }
            else {
                let i = this.users.findIndex((x:any) => x.userId === user.userId);
                this.users[i] = user;
                this.CancelEdit();
            }
        });
    }

    CancelEdit() {
        this.editUser = {
            userId: 0,
            username: "",
            role: "",
            teamId: 0,
            firstName: "",
            lastName: "",
            isActive: true, 
            calibrationPosition: 0,
        }
    }

    Cancel() {
        this.addUser = false;
        this.ResetCreateUser();
    }

    minPosition(user:IUser) {
        var minPosition = Math.min(...this.users.map((x:IUser) => x.calibrationPosition));
        return (user.calibrationPosition == minPosition);
    }

    maxPosition(user:IUser) {
        var maxPosition = Math.max(...this.users.map((x:IUser) => x.calibrationPosition));
        return (user.calibrationPosition == maxPosition);
    }

    MoveDown(user:IUser) {
        if (this.maxPosition(user))
            return;

        this.users[user.calibrationPosition].calibrationPosition -= 1;
        this.users[user.calibrationPosition - 1].calibrationPosition += 1;

        this.users = this.users.sort((a:IUser,b:IUser) => a.calibrationPosition - b.calibrationPosition);
    }

    MoveUp(user:IUser) {
        if (this.minPosition(user))
            return;

        this.users[user.calibrationPosition - 2].calibrationPosition += 1;
        this.users[user.calibrationPosition - 1].calibrationPosition -= 1;
        
        this.users = this.users.sort((a:IUser,b:IUser) => a.calibrationPosition - b.calibrationPosition);
    }
}