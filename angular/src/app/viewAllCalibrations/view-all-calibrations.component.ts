import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ICalibration } from 'src/interfaces/calibration';
import { CalibrationService } from 'src/services/calibration.service';

@Component ({
    templateUrl: 'view-all-calibrations.component.html',
    styleUrls: ['view-all-calibrations.component.css']
})

export class ViewAllCalibrations {
    calibrations:ICalibration[] = [];
    calibration:any;
    
    constructor(private _calibrationService: CalibrationService, private router:Router){
    }
    
    ngOnInit() {
        this._calibrationService.getAllCalibrations().subscribe(data => {
            this.calibrations = data;
        });
    }

    ConvertToPercent(earned:number, possible: number) {
        return earned / possible * 100;
    }

    PushToOne(id:number) {
        this.router.navigate(['/view/',id]);
    }

    PushToGroup(id:number) {
        this.router.navigate(['/group/',id]);
    }
}