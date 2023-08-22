import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { IRegisterUser } from "src/interfaces/registerUser";
import { AuthService } from "./auth.service";
import { IUser } from "src/interfaces/user";

@Injectable()
export class UserService {
    private url:string = "https://localhost:44329/";
    private httpOptions:any;
    
    constructor(private http:HttpClient, private authService:AuthService) {
        this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this.authService.currentUser.token})};
    }

    getTeams() {
        let path = this.url + "Login/Teams";
        return this.http.get(path, this.httpOptions);
    }

    getAllUsers() {
        let path = this.url + "Login/Users";
        return this.http.get<IUser>(path, this.httpOptions);
    }

    switchActive(userId:number) {
        let path = this.url + "Login/" + userId;
        return this.http.put(path, this.httpOptions)
    }

    createUser(user:IRegisterUser) {
        let path = this.url + "Login/register";
        return this.http.post(path, user, this.httpOptions);
    }

    updateUser(user:IUser) {
        let path = this.url + "Login";
        return this.http.post(path, user, this.httpOptions);
    }
}