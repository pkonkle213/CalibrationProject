import { Component } from "@angular/core";
import { StatsService } from "src/services/statistics.service";
import { CalibrationService } from "src/services/calibration.service";

@Component({
    selector: 'view-stats',
    templateUrl: 'view-stats.component.html',
    styleUrls: ['view-stats.component.css'],
})

export class ViewStatsComponent {
    overallCalibrated:any;
    questions:any;
    questionCalibrated:any[] = [];
    calibrations:any;
    calibrationCalibrated:any[] = [];
    types:any;
    typeCalibrated:any[] = [];
    questionVisible:boolean = false;
    calibrationVisible:boolean = false;
    typeVisible:boolean = false;

    constructor(private _statsService:StatsService, private _calibrationService:CalibrationService){
        
    }

    ngOnInit() {
        this._statsService.getOverallCalibrated().subscribe(data => {
            this.overallCalibrated = data;
        });

        this._statsService.getAllQuestions().subscribe(data => {
            this.questions = data;

            for(let i = 0; i < this.questions.length; i++) {
                this._statsService.getQuestionCalibrated(this.questions[i].id).subscribe(data => {
                    this.questionCalibrated.push(data);
                });
            }
        });

        this._calibrationService.getAllCalibrations().subscribe(data => {
            this.calibrations = data;

            for(let i = 0; i < this.calibrations.length; i++) {
                this._statsService.getCalibrationCalibrated(this.calibrations[i].id).subscribe(data => {
                    this.calibrationCalibrated.push(data);
                });
            }
        });

        this._calibrationService.getAllContactTypes().subscribe(data => {
            this.types = data;

            for(let i = 0; i < this.types.length; i++) {
                this._statsService.getTypeCalibrated(this.types[i].id).subscribe(data => {
                    this.typeCalibrated.push(data);
                });
            }
        });
    }

    ChangeQuestionVisible() {
        this.questionVisible = !this.questionVisible;
    }

    ChangeCalibrationVisible() {
        this.calibrationVisible = !this.calibrationVisible;
    }

    ChangeTypeVisible() {
        this.typeVisible = !this.typeVisible;
    }

    TotalCalibrated(correct:number,possible:number) {
        if(possible===null || possible===0){
            return "None attempted";
        }
        return correct / possible * 100 + "%";
    }
}