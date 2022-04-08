import { Component } from "@angular/core";
import { StatsService } from "src/services/statistics.service";

@Component({
    selector: 'view-stats',
    templateUrl: 'view-stats.component.html',
    styleUrls: ['view-stats.component.css'],
})

export class ViewStatsComponent {
    totalSuccess:number = 0;
    totalPossible:number = 0;
    questions:any;
    myAnswers:any;
    groupAnswers:any;

    constructor(private _statsService:StatsService){
        
    }

    TotalCalibrated(){
        return this.totalSuccess / this.totalPossible * 100;
    }

    ngOnInit() {
        this._statsService.getAllQuestions().subscribe(data => {
            this.questions = data;
            console.log(this.questions);
        });

        this._statsService.getGroupAnswers().subscribe(data => {
            this.groupAnswers = data;
            console.log(this.groupAnswers);
        });

        this._statsService.getMyAnswers().subscribe(data => {
            this.myAnswers = data;
            console.log(this.myAnswers);
        });
    }

    Calibrated(id:number) {
        let success=0;
        let possible=0;

        for(let me=0;me<this.myAnswers.length;me++) {
            if (this.myAnswers[me].questionId===id) {
                for(let group=0;group<this.groupAnswers.length;group++) {
                    if(this.groupAnswers[group].calibrationId===this.myAnswers[me].calibrationId && this.groupAnswers[group].questionId===this.myAnswers[me].questionId) {
                        this.totalPossible++;
                        possible++;
                        if(this.groupAnswers[group].optionValue===this.myAnswers[me].optionValue) {
                            this.totalSuccess++;
                            success++;
                        }
                    }
                }
            }
        }

        return success / possible * 100;
    }
}