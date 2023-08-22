import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { catchError, of, tap } from "rxjs";
import { IReturn } from "src/interfaces/returnUser";

@Injectable()
export class AuthService {
    currentUser:IReturn = {
        user: {
            userId: 0,
            role: "",
            teamId: 0,
            username: "",
            firstName: "",
            lastName: "",
            isActive: false,
        },
        token: "",
    }
    private url:string = "https://localhost:44329/";
    
    constructor(private http:HttpClient) {
    
    }

    loginUser(userName:string, password:string) {
        let options = { headers: new HttpHeaders({'Content-Type': 'application/json'})};
        let login = this.url + "Login";
        let sendUser = { username: userName, password: password}
        return this.http.post<any>(login,sendUser,options)
            .pipe(tap(data => {
                this.currentUser = <IReturn>data;
            }))
            .pipe(catchError(err => {
                return of(false)
            }));
    }

    isAuthenticated() {
        return this.currentUser.user.userId>0;
    }

    LeaderCheck(leaderId:number) {
        return (leaderId === this.currentUser.user.userId);
    }

    AdminCheck() {
        return (this.currentUser.user.role === "Admin");
    }

    logOutUser() {
        this.currentUser={
            user: {
                userId: 0,
                role: "",
                teamId: 0,
                username: "",
                firstName: "",
                lastName: "",
                isActive: true,
            },
            token: "",
        }; 
    }
}