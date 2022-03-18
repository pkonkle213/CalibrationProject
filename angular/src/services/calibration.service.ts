import { HttpClient, HttpHeaders } from "@angular/common/http"
import { catchError, Observable, of } from "rxjs";
import { Injectable } from "@angular/core";
import { IQuestion } from '../interfaces/question';
import { ICalibration } from '../interfaces/calibration';
import { IAnswer } from "src/interfaces/answer";

@Injectable()
export class CalibrationService {
    private url:string = "https://localhost:44329/";
    
    constructor(private http:HttpClient){

    }
    
    // private handleError<T> (operation = 'operation', result?: T){
    //     return (error: any): Observable<T> => {
    //         console.error(error);
    //         return of (result as T);
    //     }
    // }
    
    getAllCalibrations():Observable<ICalibration[]> {
        let calibrations = this.url + "Calibration/All"
        return this.http.get<ICalibration[]>(calibrations);
    }
    
    getCalibration(calibrationId:number) {
        let calibration = this.url + "Calibration/" + calibrationId;
        return this.http.get<ICalibration>(calibration);
    }

    getQuestions(calibrationId:number) {
        let questions = this.url + "Question/" + calibrationId;
        return this.http.get<IQuestion>(questions);
    }

    getMyAnswers(calibrationId:number) {
        let answers = this.url + "Answer/" + calibrationId;
        return this.http.get<IAnswer[]>(answers);
    }

    submitMyAnswer(answers:IAnswer[]) {
        let options = { headers: new HttpHeaders({'Content-Type': 'application/json'})};
        let submit = this.url + "Answer/Answer";
        return this.http.post(submit,answers,options);
    }

    updateMyAnswer(answers:IAnswer[]) {
        let options = { headers: new HttpHeaders({'Content-Type': 'application/json'})};
        let submit = this.url + "Answer/Answer";
        return this.http.put(submit,answers,options)

    }

    getParticipants(calibrationId:number) {
        let participants = this.url + "Answer/Participants/" + calibrationId;
        return this.http.get(participants);
    }

    getGroupAnswers(calibrationId:number) {
        let groupAnswer = this.url + "Answer/Group/" + calibrationId;
        return this.http.get(groupAnswer);
    }
}