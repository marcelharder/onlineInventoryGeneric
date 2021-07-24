import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { catchError } from 'rxjs/operators';
import { Location } from '../_models/Location';
import { HospitalService } from '../_services/hospital.service';


@Injectable()
export class ListLocationsResolver implements Resolve<Location[]> {
   
    constructor(private hospitalService: HospitalService,
        private router: Router,
        private alertify: AlertifyService,
        private authService: AuthService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Location[]> {
        return this.hospitalService.getListOfHospitalsWhereVendorIsActive().pipe(
                catchError(error => {
                    this.alertify.error('Problem retrieving data');
                    this.router.navigate(['/home']);
                    return of(null);
                })
            );

    }
}