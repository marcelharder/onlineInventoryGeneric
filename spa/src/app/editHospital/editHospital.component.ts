import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Location } from '../_models/Location';
import { GeneralService } from '../_services/general.service';
import { HospitalService } from '../_services/hospital.service';
import { Router } from '@angular/router';
import { User } from '../_models/User';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-editHospital',
  templateUrl: './editHospital.component.html',
  styleUrls: ['./editHospital.component.css']
})
export class EditHospitalComponent implements OnInit  {
@Input() selectedHospital: Location;
@Input() country: string;
@Input() contactName: string;
@Input() contactNumber: number;
@Output() hospitalOut: EventEmitter<number> = new EventEmitter();
currentVendor = '';


  constructor(private gen: GeneralService,
              private auth: AuthService,
              private alertify:AlertifyService,
              private hosService: HospitalService,
              private router: Router, private user: UserService) { }

  ngOnInit(): void {
    let rep: User;
    this.user.getUser(this.auth.decodedToken.nameid).subscribe((next) => {
        rep = next;this.currentVendor = rep.vendorName;
      });
  }
  
  deleteVendorInHospital() {
    this.hosService.removeVendor(this.currentVendor, this.selectedHospital.locationId).subscribe((next) => {
       this.hospitalOut.emit(1);
    },(error)=>{this.alertify.warning(error);});
  }

  updateHospitalDetails() {
    this.hosService.saveDetails(this.selectedHospital).subscribe((next) => {
      this.alertify.message(next);
      this.hospitalOut.emit(1);
  });
    
   
  
  }
}

