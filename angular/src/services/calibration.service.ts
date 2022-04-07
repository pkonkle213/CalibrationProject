import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { IQuestion } from '../interfaces/question';
import { ICalibration } from '../interfaces/calibration';
import { IAnswer } from "src/interfaces/answer";
import { IScore } from "src/interfaces/score";
import { AuthService } from 'src/services/auth.service';
import { IContactOption } from "src/interfaces/contactOption";

@Injectable()
export class CalibrationService {
    private url:string = "https://localhost:44329/";
    private httpOptions:any;
    
    constructor(private http:HttpClient,private _authService:AuthService){
        this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this._authService.currentUser.token})};
    }
    
    // private handleError<T> (operation = 'operation', result?: T){
    //     return (error: any): Observable<T> => {
    //         console.error(error);
    //         return of (result as T);
    //     }
    // }

    getAllContactTypes() {
        let options = this.url + "Calibration/Types";
        return this.http.get<IContactOption>(options,this.httpOptions);
    }

    createCalibration(calibration:ICalibration) {
        let create = this.url + "Calibration";
        return this.http.post<ICalibration>(create,calibration,this.httpOptions)
    }
    
    getAllCalibrations() {
        let calibrations = this.url + "Calibration/All"
        return this.http.get<ICalibration[]>(calibrations,this.httpOptions);
    }
    
    getCalibration(calibrationId:number) {
        let calibration = this.url + "Calibration/" + calibrationId;
        return this.http.get<ICalibration>(calibration,this.httpOptions);
    }

    getQuestions(calibrationId:number) {
        let questions = this.url + "Question/" + calibrationId;
        return this.http.get<IQuestion>(questions,this.httpOptions);
    }

    getMyScores() {
        let scores = this.url + "Calibration/Scores";
        return this.http.get<IScore[]>(scores,this.httpOptions);
    }

    getMyAnswers(calibrationId:number) {
        let answers = this.url + "Answer/" + calibrationId;
        return this.http.get<IAnswer[]>(answers,this.httpOptions);
    }

    submitMyAnswer(answers:IAnswer[]) {
        let submit = this.url + "Answer/Answer";
        return this.http.post(submit,answers,this.httpOptions);
    }

    submitMyScore(score:IScore) {
        let submit = this.url + "Answer/Score";
        return this.http.post(submit,score,this.httpOptions);
    }

    updateMyAnswer(answers:IAnswer[]) {
        let submit = this.url + "Answer/Answer";
        return this.http.put(submit,answers,this.httpOptions)
    }

    updateMyScore(score:IScore) {
        let submit = this.url + "Answer/Score";
        return this.http.put(submit,score,this.httpOptions);
    }

    getParticipants(calibrationId:number) {
        let participants = this.url + "Answer/Participants/" + calibrationId;
        return this.http.get(participants,this.httpOptions);
    }

    getGroupAnswers(calibrationId:number) {
        let groupAnswer = this.url + "Answer/Group/" + calibrationId;
        return this.http.get(groupAnswer,this.httpOptions);
    }

    switchIsOpen(calibrationId:number) {
        let switchOpen = this.url + "Calibration/" + calibrationId;
        return this.http.put(switchOpen,this.httpOptions);
    }
}