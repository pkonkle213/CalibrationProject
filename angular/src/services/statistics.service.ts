import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { IQuestion } from "src/interfaces/question";
import { AuthService } from 'src/services/auth.service';

@Injectable()
export class StatsService {
    private url:string = "https://localhost:44329/";
    private httpOptions:any;
    
    constructor(private http:HttpClient,private _authService:AuthService){
        this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this._authService.currentUser.token})};
    }

    getAllQuestions() {
        let questions = this.url + "Stats/Question";
        return this.http.get<IQuestion>(questions,this.httpOptions);
    }

    getMyAnswers() {
        let individual = this.url + "Stats/Individual";
        return this.http.get<IQuestion>(individual,this.httpOptions);
    }

    getGroupAnswers() {
        let group = this.url + "Stats/Group";
        return this.http.get<IQuestion>(group,this.httpOptions);
    }
}