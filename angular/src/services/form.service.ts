import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from "./auth.service";
import { IForm } from "src/interfaces/form";

@Injectable()
export class FormService {
    private url:string = "https://localhost:44329/";
    private httpOptions:any;

    constructor(private http:HttpClient, private _authService:AuthService) {
        this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json', 'Authorization': 'Bearer ' + this._authService.currentUser.token})};
    }

    getAllForms() {
        let path = this.url + "Form/All";
        return this.http.get<IForm[]>(path, this.httpOptions);
    }

    getActiveForms() {
        let path = this.url + "Form/Active";
        return this.http.get<IForm[]>(path, this.httpOptions);
    }

    createForm(form:IForm) {
        let path = this.url + "Form";
        return this.http.post<IForm>(path, form, this.httpOptions);
    }

    switchIsArchived(formId:number) {
        let path = this.url + "Form/Disable/" + formId;
        return this.http.put(path, this.httpOptions);
    }

    deleteForm(formId:number) {
        let path = this.url + "Form/" + formId;
        return this.http.delete(path, this.httpOptions);
    }

    updateFormName(form:IForm) {
        let path = this.url + "Form/UpdateName";
        return this.http.put(path, form, this.httpOptions);
    }
}