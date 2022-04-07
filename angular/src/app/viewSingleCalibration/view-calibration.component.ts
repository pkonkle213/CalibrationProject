import { Component } from "@angular/core";
import { Router } from '@angular/router';
import { ActivatedRoute } from "@angular/router";
import { CalibrationService } from "src/services/calibration.service";
import { IAnswer } from "src/interfaces/answer";
import { IScore } from "src/interfaces/score";

@Component ({
    selector: 'view-single-calibration',
    templateUrl: './view-calibration.component.html',
    styleUrls:['./view-calibration.component.css']
})

export class ViewSingleCalibrationComponent {
    userId:number = 2;
    calibration:any;
    questions:any;
    answers:any;
    answerSubmit:IAnswer[] = [];
    updating:boolean = false;
    score:IScore = {
        calibrationId: 0,
        pointsEarned: 0,
        pointsPossible: 0,
    };

    constructor(private _calibrationService:CalibrationService, private _route:ActivatedRoute,private _router:Router) {

    }

    ngOnInit() {
        this._calibrationService.getCalibration(this._route.snapshot.params['id']).subscribe(data => {
            this.calibration = data;
        });

        this._calibrationService.getQuestions(this._route.snapshot.params['id']).subscribe(data => {
            this.questions = data;

            this._calibrationService.getMyAnswers(this._route.snapshot.params['id']).subscribe(data => {
                this.answers = data;

                if (this.answers===null || this.answers.length===0) {
                    for(let i=0;i<this.questions.length;i++)
                    {
                        this.answerSubmit.push({
                            calibrationId: this._route.snapshot.params['id'],
                            questionId: this.questions[i].id,
                            optionValue: this.questions[i].options[0].optionValue,
                            comment: '',
                            pointsEarned: this.questions[i].options[0].pointsEarned,
                        });
                    }
                }
                else {
                    this.answerSubmit = this.answers;
                    this.updating=true;
                }
            });
        });
    }

    onChange(event:any,i:number) {
        let q = this.questions[i].options.find((x: { optionValue: any; }) => x.optionValue===event.target.value)
        this.answerSubmit[i].pointsEarned = q.pointsEarned;
    }

    Earned() {
        let earned = 0;
        for(let i=0;i<this.questions.length;i++) {
            earned += this.answerSubmit[i].pointsEarned * this.questions[i].pointsPossible;
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

    BuildScore():IScore {
        this.score.calibrationId = this._route.snapshot.params['id'];
        this.score.pointsEarned = this.Earned();
        this.score.pointsPossible = this.Possible();
        return this.score;
    }

    SubmitAnswer() {
        console.log(this.BuildScore());
        if(this.updating) {
            this._calibrationService.updateMyAnswer(this.answerSubmit).subscribe(() => {});
            this._calibrationService.updateMyScore(this.BuildScore()).subscribe(() => {
                this._router.navigate(['/view']);
            });
        }
        else {
            this._calibrationService.submitMyAnswer(this.answerSubmit).subscribe(() => {});
            this._calibrationService.submitMyScore(this.BuildScore()).subscribe(() => {
                this._router.navigate(['/view']);
            });
        }
    }
}