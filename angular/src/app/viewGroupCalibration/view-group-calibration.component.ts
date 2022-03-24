import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from "@angular/router";
import { CalibrationService } from 'src/services/calibration.service';
import { IAnswer } from 'src/interfaces/answer';

@Component({
    selector: 'view-group-calibration',
    templateUrl: 'view-group-calibration.component.html',
    styleUrls: ['view-group-calibration.component.css'],
})

export class GroupCalibrationComponent {
    calibration:any;
    calibrationId:number;
    questions:any;
    answers:any;
    groupSubmit:IAnswer[] = [];
    updatingAnswer:boolean = false;
    participants:any;

    constructor(private _calibrationService:CalibrationService, private _route:ActivatedRoute,private _router:Router) {
        this.calibrationId=this._route.snapshot.params['id'];
    }

    ngOnInit() {
        this._calibrationService.getCalibration(this.calibrationId).subscribe(data => {
            this.calibration = data;
        });

        this._calibrationService.getQuestions(this.calibrationId).subscribe(data => {
            this.questions = data;
        });

        this._calibrationService.getParticipants(this.calibrationId).subscribe(data => {
            this.participants = data;
        });

        this._calibrationService.getGroupAnswers(this.calibrationId).subscribe(data => {
            this.answers = data;
        })
    }

    Same(index:number) {
        let count = 0;
        for(let i=0;i<this.participants.length;i++) {
            if(this.participants[i].answers[index].optionValue===this.answers[index].optionValue) {
                count++;
            }
        }
        return count;
    }

    Different(index:number) {
        let count = 0;
        for(let i=0;i<this.participants.length;i++) {
            if(this.participants[i].answers[index].optionValue!==this.answers[index].optionValue) {
                count++;
            }
        }
        return count;
    }

    SumSame() {
        let sum = 0;
        for(let i=0;i<this.questions.length;i++) {
            sum += this.Same(i);
        }

        return sum;
    }

    SumDifferent() {
        let sum = 0;
        for(let i=0;i<this.questions.length;i++) {
            sum += this.Different(i);
        }

        return sum;
    }

    Calibrated(person:any) {
        let sum = 0;
        for(let i=0;i<this.questions.length;i++) {
            if(person.answers[i].optionValue===this.answers[i].optionValue) {
                sum++;
            }
        }

        return sum / this.questions.length * 100;
    }

    Matching(questionId:any,answerValue:string) {
        let count = 0;
        for(let i=0;i<this.participants.length;i++) {
            if(this.participants[i].answers[questionId].optionValue===answerValue) {
                count++;
            }
        }

        return count;
    }

    Earned() {
        let earned = 0;
        for(let i=0;i<this.questions.length;i++) {
            earned += this.answers[i].pointsEarned * this.questions[i].pointsPossible;
        }

        return earned;
    }

    Possible() {
        let possible = 0;
        for(let i=0;i<this.questions.length;i++) {
            possible += this.questions[i].pointsPossible
        }

        return possible;
    }

    CalculateScore() {
        return this.Earned() / this.Possible() * 100;
    }

    SubmitGroupAnswer() {

    }
}