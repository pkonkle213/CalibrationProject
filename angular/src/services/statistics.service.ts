import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { IStatistic } from "src/interfaces/stat";
import { AuthService } from 'src/services/auth.service';

@Injectable()
export class StatsService {
    private url:string = "https://localhost:44329/";
    private httpOptions:any;
    
    constructor(private http:HttpClient, private _authService:AuthService){
        this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this._authService.currentUser.token})};
    }
    
    getOverallCalibrated() {
        let overall = this.url + "Stats/Overall";
        return this.http.get<IStatistic>(overall, this.httpOptions);
    }

    getQuestionCalibrated() {
        let specific = this.url + "Stats/Questions/";
        return this.http.get<IStatistic>(specific, this.httpOptions);
    }

    getCalibrationCalibrated() {
        let specific = this.url + "Stats/Calibrations/";
        return this.http.get<IStatistic>(specific, this.httpOptions);
    }

    getTypeCalibrated() {
        let specific = this.url + "Stats/Types/";
        return this.http.get<IStatistic>(specific, this.httpOptions);
    }
}