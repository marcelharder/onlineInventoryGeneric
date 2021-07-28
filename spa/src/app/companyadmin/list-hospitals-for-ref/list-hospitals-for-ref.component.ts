import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Location } from 'src/app/_models/Location';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { Router } from '@angular/router';
import { HospitalService } from 'src/app/_services/hospital.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { GeneralService } from 'src/app/_services/general.service';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-list-hospitals-for-ref',
  templateUrl: './list-hospitals-for-ref.component.html',
  styleUrls: ['./list-hospitals-for-ref.component.css']
})
export class ListHospitalsForRefComponent implements OnInit {
  @Input() hos: Location;
  @Output() hospitalListOut: EventEmitter<number> = new EventEmitter();
  contactName = '';
  hospitalContactNumber =  0;
  currentCountry = '';
  currentVendor = '';
  detailsPage = 0;
  selectPage = 0;
  FullHospitals: Array<Location> = [];

  selectedHospital: Location = {
    locationId: 0,
    naam: '',
    adres: '',
    postalCode: '',
    hospitalNo: 0,
    country: '',
    image: '',
    refHospitals: '',
    standardRef: '',
    email: '',
    contact: '',
    contact_image: '',
    telephone: '',
    fax: '',
    logo: '',
    mrnSample: '',
    sMS_mobile_number: '',
    sMS_send_time: '',
    triggerOneMonth: '',
    triggerTwoMonth: '',
    triggerThreeMonth: '',
    dBBackend: ''
  };
  hospitalDescription = '';

  constructor(private auth: AuthService,
              private user: UserService,
              private router: Router,
              private hosService: HospitalService,
              private alertify: AlertifyService,
              private gen: GeneralService) {

  }
  ngOnInit(): void {
    let rep: User;
    this.user.getUser(this.auth.decodedToken.nameid).subscribe((next) => {
      rep = next;
      this.currentVendor = next.vendorName;
      this.currentCountry = next.country;
      // tslint:disable-next-line:no-shadowed-variable
     // this.gen.getCountryName(rep.country).subscribe((next) => { this.currentCountry = next; });
    });
  }
  showDetails() { if (this.detailsPage === 1) { return true; } }
  showSelectPage() { if (this.selectPage === 1) { return true; } }

  selectDetails(id: number) {

    this.detailsPage = 1;
    this.selectPage = 0;

    this.hosService.getDetails(id).subscribe((next) => {

      this.selectedHospital = next;

      this.hospitalContactNumber = parseInt(this.selectedHospital.contact, 10);
      // this.auth.changeCurrentRecipient(hospitalContactNumber);
      this.user.getUser(this.hospitalContactNumber).subscribe((reponse) => {
          this.contactName = reponse.username;
          this.selectedHospital.contact_image = reponse.photoUrl;
        } );

    });


  }
  updateHospitalDetails(s: number) {
    this.detailsPage = 0;
    this.hospitalListOut.emit(1); // force a refesh for the hospitals

  }
  selectThisHospital(id: number) {
   this.hosService.addVendor(this.currentVendor, id).subscribe((next) => {
    this.selectPage = 0;
    this.hospitalListOut.emit(1); // force a refesh for the hospitals
   });
  }
  AddHospital() {
    this.detailsPage = 0;
    this.selectPage = 1;
    this.alertify.message('Adding hospital');
  }



  DisplayHospitalsInTheCurrentCountry() {
    this.detailsPage = 0;
    this.selectPage = 1;
    this.alertify.message('Display a list of hospitals where this vendor is not active, yet');
    this.hosService.getFullHospitalsWhereVendorIsNotActive().subscribe((next) => {
      this.FullHospitals = next;
    });



  }

}
