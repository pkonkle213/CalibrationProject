import { Component } from "@angular/core";
import { IQuestion } from "src/interfaces/question";
import { CalibrationService } from "src/services/calibration.service";
import { ActivatedRoute } from "@angular/router";
import { IDetail } from "src/interfaces/detail";

@Component ({
    selector: 'view-single-calibration',
    templateUrl: './view-calibration.component.html'
})

export class ViewSingleCalibrationComponent {
    questions:any = [];
    answers:any = [];
    calibration:any = [];
    userId:number = 0;
    scores:any = [];
    answerSubmit:IDetail[] = [];

    constructor(private _calibrationService:CalibrationService, private _route:ActivatedRoute) {
        this.userId=2;
        // this.calibration = this._calibrationService.getCalibration(+this._route.snapshot.params['id'])
        // this.questions = this._calibrationService.getQuestions(this.calibration.id);
        // this.scores = this._calibrationService.getMyAnswers(this.calibration.id, this.userId);
        if (this.scores==null)
        {
            for(let i=0;i<this.questions.length;i++)
            {
                this.answerSubmit.push({
                    option:"Meets",
                    comment:""
                })
            }
        }
        else
        {
            for(let i=0;i<this.questions.length;i++)
            {
                this.answerSubmit.push(this.scores.details[i])
            }
        }
    }

    ngOnInit() {
        
    }

    CheckForDeduction(index:number) {
        if (this.answerSubmit[index].option==='Does Not Meet') {
            return true;
        }

        return false;
    }

    SubmitAnswer() {
        console.log(this.answerSubmit);
        if (this.calibration!=null){
            this.calibration.isOpen=false;
        }
        // send the update to the database as well
    }
}