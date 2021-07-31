import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { HospitalService } from 'src/app/_services/hospital.service';
import { Location } from 'src/app/_models/Location';
import { DropItem } from 'src/app/_models/dropItem';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-list-locations',
  templateUrl: './list-locations.component.html',
  styleUrls: ['./list-locations.component.css']
})
export class ListLocationsComponent implements OnInit {

  selectedHospital: number;
  selectedVendor: number;
  listOfAvailableVendors: Array<DropItem> = [];
  sd = 0; adl = 0; 
  locationDetails: Location = {
    locationId: 0,
    naam: "",
    adres: "",
    postalCode: "",
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
    dBBackend: "",
    vendors: []
  }
  ah: Array<DropItem> = [];

  constructor(private route: ActivatedRoute,
    private auth: AuthService,
    private hos: HospitalService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe((next) => {
      this.ah = next.locs;



    })
  }

  showDetails() { if (this.sd === 1) return true; }
  showAddLocation() { if (this.adl === 1) return true; }
 

  hospitalChanged() {
    debugger;
    if(this.selectedHospital !== undefined){
      this.sd = 1;
      this.hos.getDetails(this.selectedHospital).subscribe((next) => {
        this.locationDetails = next;
        // get List of available vendors
        this.hos.getListOfAvailableVendors(this.locationDetails.locationId).subscribe((next) => {
           this.listOfAvailableVendors = next;
        });
      })
    } else {this.alertify.warning("No location selected ...");}
    
  }
  updateLocationDetails() {
    this.hos.saveDetails(this.locationDetails).subscribe((next) => {
      this.sd = 0;
      this.alertify.message("Updated ..");
    }, (error) => { this.alertify.error(error) });

  }
  cancel() {
    this.sd = 0;
    this.alertify.message("Cancel");
  }

  deleteLocation() { 
    let hospitalName = "";
    this.hos.getDetails(this.selectedHospital).subscribe((next)=>{
      hospitalName = next.naam;
      this.alertify.confirm("Are you sure to delete " + hospitalName, ()=>{
        this.hos.removeLocation(this.selectedHospital).subscribe(
          (next)=>{
            // hide de details sectie
            this.adl = 0;
            // remove this location from the current list of locations dropitems
            let index = this.ah.findIndex(a => a.value === this.selectedHospital);
            this.ah.splice(index, 1);
            this.alertify.message("Deleting ...");});
      });
    })
  }
  addLocation() { 
    this.adl = 1;
    this.sd = 0;
    this.hos.addLocation().subscribe((next)=>{this.locationDetails = next;});
    }

  removeVendor(Description: string) {
    debugger;
    this.hos.removeVendor(Description, this.selectedHospital).subscribe(
      (next) => {
        this.alertify.message("Updated ..");
        let index = this.locationDetails.vendors.findIndex(a => a.value === this.selectedHospital);
        this.locationDetails.vendors.splice(index, 1);
      }
    )
  }
  addVendor() {
   
    let index = this.listOfAvailableVendors.findIndex(a => a.value === this.selectedHospital);
    let dropItem = this.listOfAvailableVendors.find(a => a.value == this.selectedVendor);
    this.locationDetails.vendors.push(dropItem);
    this.listOfAvailableVendors.splice(index, 1);
  }

  saveLocation(){
    // save the details to the server
    this.hos.saveDetails(this.locationDetails).subscribe((next)=>{
      this.adl = 0;
      this.sd = 0;
      // push the new location record on the list of locations
      let cl:DropItem = {value: this.locationDetails.locationId, description: this.locationDetails.naam};
      this.ah.push(cl);
      this.alertify.message("Saving ...");
    }, (error)=>{this.alertify.error(error);})

    
   }

  showDetailsVendor(Value: number) { }


}
