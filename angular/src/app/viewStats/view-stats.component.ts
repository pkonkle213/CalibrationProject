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
    questionCalibrated:any;
    calibrationCalibrated:any;
    typeCalibrated:any;
    questionVisible:boolean = false;
    calibrationVisible:boolean = false;
    typeVisible:boolean = false;

    constructor(private _statsService:StatsService){
        
    }

    ngOnInit() {
        this._statsService.getOverallCalibrated().subscribe(data => {
            this.overallCalibrated = data;
        });

        this._statsService.getQuestionCalibrated().subscribe(data => {
            this.questionCalibrated = data;
        });

        this._statsService.getCalibrationCalibrated().subscribe(data => {
            this.calibrationCalibrated = data;
        });

        this._statsService.getTypeCalibrated().subscribe(data => {
            this.typeCalibrated = data;
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

    TotalCalibrated(correct:number, possible:number) {
        if(possible === null || possible === 0){
            return "None attempted";
        }

        return correct / possible * 100 + "%";
    }
}