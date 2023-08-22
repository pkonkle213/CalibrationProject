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
    }

    editUser:IUser = {
        userId: 0,
        username: "",
        role: "",
        teamId: 0,
        firstName: "",
        lastName: "",
        isActive: true, 
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
        console.log(user);
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
        };
    }

    SwitchUserIsActive(user:IUser) {
        this.userService.switchActive(user.userId).subscribe((data) => {
            if(!data){
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
        }
    }

    Cancel() {
        this.addUser = false;
        this.ResetCreateUser();
    }
}