import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { DropItem } from '../_models/dropItem';

@Injectable()
export class DropService {
    [x: string]: any;
    baseUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    getImplantedOptions() {   return this.http.get<DropItem[]>(this.baseUrl + 'options_implanted'); }
    getValveLocationOptions() {return this.http.get<DropItem[]>(this.baseUrl + 'options_product_location'); }
    getValveTypeOptions() {return this.http.get<DropItem[]>(this.baseUrl + 'options_product_type'); }
    getRoleOptions() {return this.http.get<DropItem[]>(this.baseUrl + 'options_role'); }
    getCompanyOptions(){return this.http.get<DropItem[]>(this.baseUrl + 'options_companies'); }
    getAllHospitals(){return this.http.get<DropItem[]>(this.baseUrl + 'allHospitals');}

}
