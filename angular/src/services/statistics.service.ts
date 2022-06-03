import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ICalibrated } from "src/interfaces/calibrated";
import { ICalibration } from "src/interfaces/calibration";
import { IContactType } from "src/interfaces/contactType";
import { IQuestion } from "src/interfaces/question";
import { AuthService } from 'src/services/auth.service';

@Injectable()
export class StatsService {
    private url:string = "https://localhost:44329/";
    private httpOptions:any;
    
    constructor(private http:HttpClient,private _authService:AuthService){
        this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this._authService.currentUser.token})};
    }
    
    getOverallCalibrated() {
        let overall = this.url + "Stats/Overall";
        return this.http.get<ICalibrated>(overall,this.httpOptions);
    }

    getAllQuestions() {
        let questions = this.url + "Stats/Question";
        return this.http.get<IQuestion>(questions,this.httpOptions);
    }

    getQuestionCalibrated(questionId:number) {
        let specific = this.url + "Stats/Question/" + questionId;
        return this.http.get<ICalibrated>(specific,this.httpOptions);
    }

    getCalibrationCalibrated(questionId:number) {
        let specific = this.url + "Stats/Calibration/" + questionId;
        return this.http.get<ICalibrated>(specific,this.httpOptions);
    }

    getTypeCalibrated(questionId:number) {
        let specific = this.url + "Stats/Type/" + questionId;
        return this.http.get<ICalibrated>(specific,this.httpOptions);
    }
}