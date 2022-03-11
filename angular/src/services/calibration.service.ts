import { HttpClient } from "@angular/common/http"
import { catchError, Observable, of } from "rxjs";
import { Injectable } from "@angular/core";
import { IAnswer } from '../interfaces/answer';
import { IParticipant } from '../interfaces/participant';
import { IQuestion } from '../interfaces/question';
import { ICalibration } from '../interfaces/calibration';
import { IUser } from "src/interfaces/user";

@Injectable()
export class CalibrationService {
    private url: string = "https://localhost:44329/";
    // handleError = '';
    
    constructor(private http:HttpClient){}
    
    getAllCalibrations():Observable<ICalibration[]> {
        let calibrations = this.url + "Calibration/All"
        return this.http.get<ICalibration[]>(calibrations)
            .pipe(catchError(this.handleError<ICalibration[]>('getAllCalibrations',[])))
    }
    
    private handleError<T> (operation = 'operation', result?: T){
        return (error: any): Observable<T> => {
            console.error(error);
            return of (result as T);
        }
    }

    // getParticipants(calibrationId:number):Observable<IParticipant[]> {
    //     let participants = this.url + "Answer/participants";
    //     return this.http.get<IParticipant[]>(participants)
    //     // return PARTICIPANTS;
    // }
    
    // getAnswers(calibrationId:number) {
    //     return ANSWERS;
    // }
    
    // getMyAnswers(calibrationId:number, userId:number) {
    //     return ANSWERS.find(answer => answer.participantId === userId);
    // }
    
    // getQuestions(calibrationId:number) {
    //     return QUESTIONS;
    // }

    // getCalibration(calibrationId:number) {
    //     return ALLCALIBRATIONS.find(calibration => calibration.id === calibrationId);
    // }

    // getGroupScore(calibrationId:number) {
    //     return GROUPSCORE;
    // }
}

const ANSWERS:IAnswer[] = [
    {
        participantId: 1,
        details:
        [
            {
                option: "Meets",
                comment: "Sometimes I do",
            },
            {
                option: "Does Not Meet",
                comment: "Personal bias, it should be Alex",
            },
            {
                option: "Meets",
                comment: "Idk",
            },
        ]
    },
    {
        participantId: 2,
        details:
        [
            {
                option: "Does Not Meet",
                comment: "Irrelevant to the situation",
            },
            {
                option: "Meets",
                comment: "Absolutely, I am.",
            },
            {
                option: "Does Not Meet",
                comment: "Because I'm super mean and stuff",
            },
        ]
    },
]

const ALLCALIBRATIONS:ICalibration[] = [
    {
        id: 3,
        repFirstName: "Sarah",
        repLastName: "Franklin",
        groupScoreEarned: 100,
        groupScorePossible: 100,
        contactChannel: "Tier 2 - Phones",
        calibrationDate: new Date('03/07/2022'),
        contactId: "Call 8675309",
        isOpen: true,
    },
    {
        id: 2,
        repFirstName: "Ashley",
        repLastName: "Davis",
        groupScoreEarned: 25,
        groupScorePossible: 100,
        contactChannel: "Tier 1 - Chat",
        calibrationDate: new Date('03/05/2022'),
        contactId: "Chat 987654",
        isOpen: false,
    },
    {
        id: 1,
        repFirstName: "Jean",
        repLastName: "Rowe",
        groupScoreEarned: 70,        
        groupScorePossible: 75,
        contactChannel: "Tier 2 - Back Office",
        calibrationDate: new Date('03/02/2022'),
        contactId: "Email 12345",
        isOpen: false,
    }
]

const QUESTIONS:IQuestion[] = [
    {
        id: 1,
        question: "Do you believe in love after love?",
    },
    {
        id: 3,
        question: "Was Phillip the best QA ever?",
    },
    {
        id: 7,
        question: "Is the sky blue?",
    },
]

const GROUPSCORE:IAnswer = {
    participantId: 0,
    details: [
        {
            option: "Group1 - Meets",
            comment: "Sometimes I do",
        },
        {
            option: "Group2 - Does Not Meet",
            comment: "Personal bias, it should be Alex",
        },
        {
            option: "Group3",
            comment: "Idk",
        },
    ]
}

const PARTICIPANTS:IParticipant[] = [
    {
        id: 1,
        firstName: "Alex",
        team: "QA",
        role: "Admin",
    },
    {
        id: 2,
        firstName: "Phillip",
        team: "QA",
        role: "Leader",
    },
]