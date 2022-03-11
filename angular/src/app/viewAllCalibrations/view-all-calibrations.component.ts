import { Component, OnInit } from '@angular/core';
import { ICalibration } from 'src/interfaces/calibration';
import { CalibrationService } from 'src/services/calibration.service';

@Component ({
    templateUrl: 'view-all-calibrations.component.html',
    styleUrls: ['view-all-calibrations.component.css']
})

export class ViewAllCalibrations {
    calibrations:ICalibration[] = [];
    calibration:any;
    
    constructor(private _calibrationService: CalibrationService){
    }
    
    ngOnInit() {
        this._calibrationService.getAllCalibrations().subscribe(data => {
            this.calibrations = data;
            console.log(data);
            console.log(data.length);
        });
        // console.log(this.calibrations);
    }

    ConvertToPercent(earned:number, possible: number) {
        return earned/possible*100;
    }
}