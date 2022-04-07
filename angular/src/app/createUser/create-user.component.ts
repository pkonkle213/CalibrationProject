import { Component } from '@angular/core'
import { ISendUser } from 'src/interfaces/sendUser'
import { UserService } from 'src/services/user.service';

@Component({
    selector: 'create-user',
    templateUrl: 'create-user.component.html',
    styleUrls: ['create-user.component.css'],
})

export class CreateUserComponent {
    newUser:ISendUser = {
        username: "",
        password: "password",
        confirmPassword: "password",
        role: "",
        team: "",
        first: "",
        last: ""
    }
    roles:any;
    teams:any;

    constructor(private userService:UserService) {
        this.userService.getTeams().subscribe(data => {
            this.teams = data;
        });

        this.userService.getRoles().subscribe(data => {
            this.roles = data;
        });
    }

    CreateNewUser(user:ISendUser) {
        this.userService.createUser(user).subscribe(() => {
            this.newUser = {
                username: "",
                password: "password",
                confirmPassword: "password",
                role: "",
                team: "",
                first: "",
                last: ""
            }
        });
    }
}