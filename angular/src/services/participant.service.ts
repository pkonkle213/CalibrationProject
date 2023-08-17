import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from "./auth.service";
import { IParticipant } from "src/interfaces/participant";

@Injectable()
export class ParticipantService {
    private url:string = "https://localhost:44329/";
    private httpOptions:any;
    
    constructor(private http:HttpClient,private _authService:AuthService) {
        this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this._authService.currentUser.token})};
    }

    getTeams() {
        let path = this.url + "Participant/Teams";
        return this.http.get(path, this.httpOptions);
    }

    getRoles() {
        let path = this.url + "Participant/Roles";
        return this.http.get(path, this.httpOptions);
    }

    getAllUsers() {
        let path = this.url + "Participant/Users";
        return this.http.get<IParticipant[]>(path, this.httpOptions);
    }

    createUser(user:IParticipant) {
        let path = this.url + "Login/register";
        return this.http.post(path, user, this.httpOptions);
    }

    changeActive(userId:number) {
        let path = this.url + "Participant/Active";
        return this.http.post(path, userId, this.httpOptions);
    }
}