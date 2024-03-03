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
    // userId:number = 0;
    calibrationId:number = 0;
    calibration:any;
    questions:any;
    types:any;
    answers:any;
    answerSubmit:IAnswer[] = [];
    updating:boolean = false;
    score:IScore = {
        calibrationId: 0,
        pointsEarned: 0,
        pointsPossible: 0,
    };

    constructor(private calibrationService:CalibrationService, private route:ActivatedRoute, private router:Router) {
        this.calibrationId = this.route.snapshot.params['id'];
    }

    ngOnInit() {
        this.GetCalibrationInformation();
        this.GetAllContactTypes();
        this.GetQuestionsAndAnswers();        
    }

    GetCalibrationInformation() {
        this.calibrationService.getCalibration(this.calibrationId).subscribe(data => {
            this.calibration = data;
        });
    }

    GetAllContactTypes() {
        this.calibrationService.getAllContactTypes().subscribe((data) => {
            this.types = data;
        });
    }

    GetQuestionsAndAnswers() {
        this.calibrationService.getQuestions(this.calibrationId).subscribe(data => {
            this.questions = data;

            this.calibrationService.getMyAnswers(this.calibrationId).subscribe(data => {
                this.answers = data;

                if (this.answers === null || this.answers.length === 0) {
                    for(let i = 0; i < this.questions.length; i++)
                    {
                        this.answerSubmit.push({
                            calibrationId: this.calibrationId,
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

    GetContactChannel() {
        return (this.types.find((x:any) => x.id === this.calibration.contactChannelId).name);
    }

    onChange(event:any,i:number) {
        let q = this.questions[i].options.find((x: { optionValue: any; }) => x.optionValue === event.target.value)
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
        this.score.calibrationId = this.calibrationId;
        this.score.pointsEarned = this.Earned();
        this.score.pointsPossible = this.Possible();
        return this.score;
    }

    Cancel() {
        this.router.navigate(['/view']);
    }

    SubmitAnswer() {
        if(this.updating) {
            this.calibrationService.updateMyAnswer(this.answerSubmit).subscribe(() => {});
            this.calibrationService.updateMyScore(this.BuildScore()).subscribe(() => {
                this.router.navigate(['/view']);
            });
        }
        else {
            this.calibrationService.submitMyAnswer(this.answerSubmit).subscribe(() => {});
            this.calibrationService.submitMyScore(this.BuildScore()).subscribe(() => {
                this.router.navigate(['/view']);
            });
        }
    }
}