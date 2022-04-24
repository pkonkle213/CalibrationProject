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
        lastName: ""
    }
    roles:any;
    teams:any;
    users:any;
    addUser:boolean = false;

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
                lastName: ""
            }
        });

        this.addUser=false;
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
            lastName: ""
        }
    }
}