import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { Location } from 'src/app/_models/Location';

@Component({
  selector: 'app-list-locations',
  templateUrl: './list-locations.component.html',
  styleUrls: ['./list-locations.component.css']
})
export class ListLocationsComponent implements OnInit {

  selectedHospital: number;
  sd=0;
  locationDetails: Location = {
    locationId: 0,
    naam: "",
    adres: "",
    postalCode: "",
    hospitalNo: 0,
    country: "",
    image: "",
    refHospitals: "",
    standardRef: "",
    email: "",
    contact: "",
    contact_image: "",
    telephone: "",
    fax: "",
    logo: "",
    mrnSample: "",
    sMS_mobile_number: "",
    sMS_send_time: "",
    triggerOneMonth: "",
    triggerTwoMonth: "",
    triggerThreeMonth: "",
    dBBackend: ""
  }
  ah:Array<Location> = [];

  constructor(private route: ActivatedRoute, 
    private hos: HospitalService,
    private alertify: AlertifyService) { }

  ngOnInit() {
  this.route.data.subscribe((next)=>{
    this.ah = next.locs;
  })
  }

  showDetails(){if(this.sd === 1)return true;}

  hospitalChanged(){
    this.sd = 1;
    this.alertify.message("Getting hospital details now ...");
    this.hos.getDetails(this.selectedHospital).subscribe((next)=>{
       this.locationDetails = next;
    })
  }
    updateLocationDetails(){
      this.hos.saveDetails(this.locationDetails).subscribe((next)=>{
        this.sd = 0;
        this.alertify.message("Updated ..");
      }, (error)=>{this.alertify.error(error)});
      
      }
    cancel(){ 
      this.sd = 0;
      this.alertify.message("Cancel");}
  
    deleteLocation(){this.alertify.message("Deleting ...");}
    addLocation(){this.alertify.message("Adding ...");}

    removeVendor(Description:string){}
    showDetailsVendor(Value: number){}
  

}
