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
    dBBackend: "",
    vendors: []
  }
  ah: Array<Location> = [];

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
    this.sd = 1;
    this.alertify.message("Getting hospital details now ...");
    this.hos.getDetails(this.selectedHospital).subscribe((next) => {
      this.locationDetails = next;
      // get List of available vendors
      this.hos.getListOfAvailableVendors(this.locationDetails.hospitalNo).subscribe((next) => {
         this.listOfAvailableVendors = next;
      });
    })
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

  deleteLocation() { this.alertify.message("Deleting ..."); }
  addLocation() { 
    this.adl = 1;
    this.alertify.message("Adding ..."); }

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

  showDetailsVendor(Value: number) { }


}
