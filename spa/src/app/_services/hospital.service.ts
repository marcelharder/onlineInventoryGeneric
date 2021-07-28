import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Location } from '../_models/Location';
import { DropItem } from '../_models/dropItem';

@Injectable()
export class HospitalService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getVendorsInHospital() { return this.http.get<DropItem[]>(this.baseUrl + 'hospital/vendors'); }
  getListOfHospitalsWhereVendorIsActive() { return this.http.get<DropItem[]>(this.baseUrl + 'sphlist'); }
  getHospitalFromHospitalCode(code: number) {return this.http.get<string>(this.baseUrl + 'hospitalName/' + code, { responseType: 'text' as 'json' });}
  getListOfHospitalsPerCountry(countryCode: string){ return this.http.get<DropItem[]>(this.baseUrl + 'getHospitalsInCountry/' + countryCode); } // countrycode is 31 bv
  getFullHospitalsWhereVendorIsActive() { return this.http.get<Location[]>(this.baseUrl + 'sphlist_full'); }
  getFullHospitalsWhereVendorIsNotActive() { return this.http.get<Location[]>(this.baseUrl + 'neg_sphlist_full'); }
  addVendor(vendor: string, hospitalId: number) { return this.http.get<string>(this.baseUrl + 'addVendor' + '/' + vendor + '/' + hospitalId, { responseType: 'text' as 'json' }); }
  removeVendor(vendor: string, hospitalId: number) { return this.http.get<string>(this.baseUrl + 'removeVendor' + '/' + vendor + '/' + hospitalId, { responseType: 'text' as 'json' }); }
  getDetails(hospitalId: number) { return this.http.get<Location>(this.baseUrl + 'getHospitalDetails' + '/' + hospitalId); }
  saveDetails(h: Location) { return this.http.post<string>(this.baseUrl + 'saveHospitalDetails', h, { responseType: 'text' as 'json' }); }
  isOVIPlace(){return this.http.get<number>(this.baseUrl + 'isOVIPlace');}
  getAllHospitals(){return this.http.get<DropItem>(this.baseUrl + 'allHospitals');}

}
