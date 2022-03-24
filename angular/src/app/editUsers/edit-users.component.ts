import { Component } from '@angular/core'
import { ISendUser } from 'src/interfaces/sendUser'
import { UserService } from 'src/services/user.service';

@Component({
    selector: 'edit-users',
    templateUrl: 'edit-users.component.html',
    styleUrls: ['edit-users.component.css'],
})

export class EditUsersComponent {
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
            console.log('Created!');
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