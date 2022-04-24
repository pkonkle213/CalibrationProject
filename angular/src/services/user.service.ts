import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ISendUser } from "src/interfaces/sendUser";
import { AuthService } from "./auth.service";

@Injectable()
export class UserService {
    private url:string = "https://localhost:44329/";
    private httpOptions:any;
    
    constructor(private http:HttpClient,private _authService:AuthService) {
        this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this._authService.currentUser.token})};
    }

    getTeams() {
        let path = this.url + "Login/Teams";
        return this.http.get(path,this.httpOptions);
    }

    getRoles() {
        let path = this.url + "Login/Roles";
        return this.http.get(path,this.httpOptions);
    }

    getAllUsers() {
        let path = this.url + "Login/Users";
        return this.http.get(path,this.httpOptions);
    }

        
    createUser(user:ISendUser) {
        let create = this.url + "Login/register";
        return this.http.post(create,user,this.httpOptions);
    }
}