import { Component } from '@angular/core'
import { ISendUser } from 'src/interfaces/sendUser'
import { UserService } from 'src/services/user.service';

@Component({
    selector: 'create-user',
    templateUrl: 'create-user.component.html',
    styleUrls: ['create-user.component.css'],
})

export class CreateUserComponent {
    createUser:ISendUser = {
        username: "",
        password: "password",
        confirmPassword: "password",
        role: "",
        team: "",
        firstName: "",
        lastName: "",
        isActive: true,
    }
    roles:any;
    teams:any;
    users:any;
    addUser:boolean = false;
    editId:number = 0;

    constructor(private userService:UserService) {
        this.userService.getTeams().subscribe(data => {
            this.teams = data;
        });

        this.userService.getRoles().subscribe(data => {
            this.roles = data;
        });

        this.userService.getAllUsers().subscribe(data => {
            this.users = data;
        })
    }
    
    EditCheck(userId:number) {
        return (userId===this.editId);
    }

    AddNewUser() {
        this.addUser=true;
    }

    CreateNewUser(user:ISendUser) {
        this.users.push(user);
        this.userService.createUser(user).subscribe(() => {
            this.createUser = {
                username: "",
                password: "password",
                confirmPassword: "password",
                role: "",
                team: "",
                firstName: "",
                lastName: "",
                isActive: true,
            }
        });

        this.addUser=false;
    }

    DisableUser(userId:number) {

    }

    EditUser(userId:number) {

    }

    Cancel() {
        this.addUser=false;
        this.createUser = {
            username: "",
            password: "password",
            confirmPassword: "password",
            role: "",
            team: "",
            firstName: "",
            lastName: "",
            isActive: true,
        }
    }
}