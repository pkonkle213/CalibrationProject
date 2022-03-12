import { Component } from "@angular/core";
import { IQuestion } from "src/interfaces/question";
import { CalibrationService } from "src/services/calibration.service";
import { ActivatedRoute } from "@angular/router";
import { IAnswer } from "src/interfaces/answer";
import { Router } from '@angular/router';

@Component ({
    selector: 'view-single-calibration',
    templateUrl: './view-calibration.component.html'
})

export class ViewSingleCalibrationComponent {
    userId:number = 2;
    calibration:any = [];
    questions:any = [];
    answers:any = [];
    answerSubmit:IAnswer[] = [];
    updatingAnswer:boolean = false;
    score:number = 0;

    constructor(private _calibrationService:CalibrationService, private _route:ActivatedRoute,private _router:Router) {
        this.userId=2;

    }

    ngOnInit() {
        this._calibrationService.getCalibration(this._route.snapshot.params['id']).subscribe(data => {
            this.calibration = data;
        });

        this._calibrationService.getQuestions(this._route.snapshot.params['id']).subscribe(data => {
            this.questions = data;
        });

        this._calibrationService.getMyAnswers(this._route.snapshot.params['id']).subscribe(data => {
            this.answers = data;
            
            if (this.answers.length===0) {
                for(let i=0;i<this.questions.length;i++)
                {
                    this.answerSubmit.push({
                        calibrationId: this._route.snapshot.params['id'],
                        questionId: this.questions[i].id,
                        optionValue: this.questions[i].options[0].optionValue,
                        comment: '',
                        pointsEarned: this.questions[i].options[0].pointsEarned,
                    })
                }
            }
            else {
                this.answerSubmit=this.answers;
                this.updatingAnswer=true;
            }
        });
   
    }

    CalculateScore() {
        let earned = 0;
        let possible = 0;
        for(let i=0;i<this.questions.length;i++) {
            earned += this.answerSubmit[i].pointsEarned * this.questions[i].pointsPossible;
            possible += this.questions[i].pointsPossible
        }

        return earned / possible * 100;
    }

    SubmitAnswer() {
        if(this.updatingAnswer) {
            console.log("update");
            this._calibrationService.updateMyAnswer(this.answerSubmit).subscribe(() => {
                this._router.navigate(['/view']);
            });
        }
        else {
            console.log("submit");
            this._calibrationService.submitMyAnswer(this.answerSubmit);
        }

        this._router.navigate(['/view']);
    }
}