import { Component, OnInit, ViewChild } from '@angular/core';
import { ValveService } from 'src/app/_services/valve.service';
import { NgForm } from '@angular/forms';
import { Valve } from 'src/app/_models/Valve';
import { Location } from 'src/app/_models/Location';
import { HospitalService } from 'src/app/_services/hospital.service';
import { GeneralService } from 'src/app/_services/general.service';
import { ValveTransfer } from 'src/app/_models/ValveTransfer';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-searchserial',
  templateUrl: './searchserial.component.html',
  styleUrls: ['./searchserial.component.css']
})
export class SearchserialComponent implements OnInit {
  serial = '';
  searchStarted = 0;
  valveFound = 0;
  country = '';

  transfers:Array<ValveTransfer>=[];
  selectedValve: Valve = {
    valveId: 0,
    no: 0,
    description: '',
    vendor_code: '',
    vendor_name: '',
    valveSizes: [],
    product_code: '',
    type: '',
    location: '',
    manufac_date: new Date(),
    expiry_date: new Date(),
    Implant_date: new Date(),
    serial_no: '',
    model_code: '',
    size: '',
    image:'',
    ppm: '',
    tfd:0,
    implant_position: '',
    procedure_id: 0,
    implanted: 0,
    location_code: 0
  };
  selectedHospital: Location = {
    locationId: 0,
    naam: '',
    adres: '',
    postalCode: '',
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
    dBBackend: '',
    vendors:[]
  };

  @ViewChild('editForm') editForm: NgForm;

  constructor(private valveService: ValveService,
    private alertify: AlertifyService,
    private hosService: HospitalService,
    private auth: AuthService,
    private gen: GeneralService) { }

  ngOnInit() {  }
  SearchOnSerial() {
    this.searchStarted = 1;
    this.valveService.getValveBySerial(this.serial, '1').subscribe((next) => {
      debugger;
      this.hosService.getDetails(next.location_code).subscribe((res) => {
        this.selectedHospital = res;
        this.gen.getCountryName(this.selectedHospital.country).subscribe((c) => {this.country = c; });
      });

      if (next === null) {this.valveFound = 0; } else {this.valveFound = 1; }
      this.selectedValve.valveId = next.valveId;
      this.valveService.getValveTransfers(+this.auth.decodedToken.nameid, next.valveId)
     .subscribe((nex)=>{ 
       this.transfers = nex; }, (error)=>{this.alertify.error(error)})


     }, (error) => {console.log(error); });

     

     
  }

  searchButtonPressed() {if (this.searchStarted === 1) {return true; }}
  valveIsFound() {if (this.valveFound === 1) {return true; }}
}
