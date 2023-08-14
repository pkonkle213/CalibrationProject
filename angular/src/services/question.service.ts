import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from "./auth.service";
import { IQuestion } from "src/interfaces/question";
import { ISendQuestion } from "src/interfaces/questionSend";

@Injectable()
export class QuestionService {
    private url:string = "https://localhost:44329/";
    private httpOptions:any;

    constructor(private http:HttpClient, private _authService:AuthService) {
        this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this._authService.currentUser.token})};
    }

    getQuestionsByFormId(formId:number) {
        let path = this.url + "Question/Form/" + formId;
        return this.http.get<IQuestion>(path, this.httpOptions);
    }

    submitQuestion(question:ISendQuestion) {
        let path = this.url + "Question";
        return this.http.post(path, question, this.httpOptions);
    }

    updateQuestions(questions:IQuestion[]) {
        let path = this.url + "Question";
        return this.http.put(path, questions, this.httpOptions);
    }
}