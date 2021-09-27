import { Component, OnInit, Input } from '@angular/core';
import { ValveTransfer } from 'src/app/_models/ValveTransfer';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { ValveService } from 'src/app/_services/valve.service';
import { Valve } from 'src/app/_models/Valve';
import { Router } from '@angular/router';
import { DropItem } from 'src/app/_models/dropItem';
import { UserService } from 'src/app/_services/user.service';
import { HospitalService } from 'src/app/_services/hospital.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-list_Transfers',
  templateUrl: './list_Transfers.component.html',
  styleUrls: ['./list_Transfers.component.css']
})
// tslint:disable-next-line:class-name
export class List_TransfersComponent implements OnInit {
  @Input() transfers: ValveTransfer[];
  @Input() selectedValve: Valve;
  details = 0;

  optionsDepartureHospital: Array<DropItem> = [];
  optionsDestinationHospital: Array<DropItem> = [];

  currentTransfer: ValveTransfer = {
    Id:0,
    DepTime:new Date(),
    ArrTime:new Date(),
    Reason: '',
    DepartureCode: '',
    ArrivalCode: '',
    ValveId: 0
  };

  constructor(private alertify: AlertifyService,
    private hos: HospitalService,
    private userservice: UserService,
    private auth: AuthService,
    private router: Router,
    private valveservice: ValveService) { }

  ngOnInit() {
    this.userservice.getCurrentCountryCode(+this.auth.decodedToken.nameid).subscribe((next)=>{
      const country = next;
      this.loadDrops(country);
    });
  }


  loadDrops(country: string) {
    // get the hospitals in the current country
    const d = JSON.parse(localStorage.getItem('options_departure_location'));
    if (d == null || d.length === 0) {
      this.hos.getListOfHospitalsPerCountry(country).subscribe((response) => {

            this.optionsDepartureHospital = response;
            // tslint:disable-next-line:max-line-length
            if (this.optionsDepartureHospital.includes({value: 100, description: 'Plant'}) === false){ this.optionsDepartureHospital.unshift({value: 100, description: 'Plant'}); };
            // tslint:disable-next-line:max-line-length
            if (this.optionsDepartureHospital.includes({value: 0, description: 'Store'}) === false) { this.optionsDepartureHospital.unshift({value: 0, description: 'Store'});  };

            localStorage.setItem('options_departure_location', JSON.stringify(response));

            this.optionsDestinationHospital =  this.optionsDepartureHospital;

            // If needed extra options can be inserted here

            localStorage.setItem('options_destination_location', JSON.stringify(response));
        });
    } else {
        this.optionsDepartureHospital = JSON.parse(localStorage.getItem('options_departure_location'));
        this.optionsDestinationHospital = JSON.parse(localStorage.getItem('options_destination_location'));
    }

  }

  cancel() {

    this.details = 0;
    this.router.navigate(['/search']);

  }

  addTransfer() {
    this.details = 1;
    this.alertify.message('adding record');
    // tslint:disable-next-line:max-line-length
    this.valveservice.addValveTransferDetails(+this.auth.decodedToken.nameid, this.selectedValve.valveId).subscribe((next) => {
      this.currentTransfer = next;

    })
    }

   updateTransfer(id: number) {
     this.details = 1;
     this.valveservice.getValveTransferDetails(+this.auth.decodedToken.nameid,id).subscribe((next)=>{
       this.currentTransfer = next;
     })
    }

   saveValveTransferDetails()
   {
    this.valveservice.updateValveTransferDetails(+this.auth.decodedToken.nameid, this.currentTransfer).subscribe((next) => {
       this.alertify.message('updating record');
       
       // update the local list of transfers with the newly added one
       // index = this.transfers.findIndex(i => i.Id === this.currentTransfer.Id);
       //this.transfers.splice(index,1);
       //this.transfers.push(this.currentTransfer);
       // order this list by Id
       //this.transfers.sort(i => i.Id);

       // insert the new transfer in place of the old one
       this.transfers = this.transfers.map(u => u.Id !== this.currentTransfer.Id ? u : this.currentTransfer);
       
       this.details = 0; // show the list of transfers again
      }, (error)=>{this.alertify.error(error)})
   }





  showDetails() { if (this.details === 1) { return true; } }

}
