import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { Valve } from '../_models/Valve';
import { DropItem } from '../_models/dropItem';
import { GeneralService } from '../_services/general.service';
import { AlertifyService } from '../_services/alertify.service';
import { DropService } from '../_services/drop.service';
import { AuthService } from '../_services/auth.service';
import * as moment from 'moment';
import { ValveTransfer } from '../_models/ValveTransfer';
import { ValveService } from '../_services/valve.service';
import { valveSize } from '../_models/valveSize';
import { ProductService } from '../_services/product.service';
import { TypeOfValve } from '../_models/TypeOfValve';
import { HospitalService } from '../_services/hospital.service';


@Component({
    selector: 'app-edit-valve-in-hospital',
    templateUrl: './edit-valve-in-hospital.component.html'
})

export class EditValveInHospitalComponent implements OnInit {

    @Input() valve: Valve;
    @Input() sizes: number[];
    @Output() valveBack = new EventEmitter<Valve>();
    optionsImplant: Array<DropItem> = [];
    optionsSizes: Array<DropItem> = [];
    ch = 0;
    product: TypeOfValve =  {
        valveTypeId: 0,
        no: 0,
        uk_code: '',
        us_code: '',
        description: '',
        product_size: [],
        type: '',
        image: '',
        vendor_description: '',
        vendor_code: '',
        model_code: '',
        implant_position: '',
        countries: ''
   };

    HospitalName = '';
    VendorName = '';

    // sendPanel = 0;

    constructor(
        private gen: GeneralService,
        private auth: AuthService,
        private alertify: AlertifyService,
        private router: Router,
        private hos: HospitalService,
        private prod: ProductService,
        private valveService: ValveService,
        private drops: DropService) {

    }
    ngOnInit(): void {
        // get the hospitalName from the auth service, because the valve is not here yet
        this.auth.currentHospital.subscribe((next) => { 
            let hospitalNumber = next;
            this.hos.getDetails(+hospitalNumber).subscribe((nex)=>{
                this.HospitalName = nex.naam;
            })
            
             });

        this.prod.getValveSizes(this.valve.model_code).subscribe((nex)=>{
            this.optionsSizes = nex;
        })
        this.loadDrops();
    }

    displayChangeSize() { if (this.ch === 1) { return true; } }

    superUserLoggedin() { if (this.auth.decodedToken.role === 'superuser') { return true; } else { return false; } }

    Cancel() { this.router.navigate(['/home']); }

    Save() {
        if (this.canIGo()) {
            this.valveBack.emit(this.valve);
        } else {
            // log the error;

        }


    }

    selectThisValve(id: number) {
        this.ch = 0;
        this.valve.size = id.toString();
    }
    


    loadDrops() {
        const d = JSON.parse(localStorage.getItem('implant_options'));
        if (d == null || d.length === 0) {
            this.drops.getImplantedOptions().subscribe((response) => {
                this.optionsImplant = response; localStorage.setItem('implant_options', JSON.stringify(response));
            });
        } else {
            this.optionsImplant = JSON.parse(localStorage.getItem('implant_options'));
        }
    }

    canIGo(): boolean {
        let help = false;

        if (this.valve.manufac_date.toString() === '0001-01-01T00:00:00') { this.alertify.error('Please enter manufac date ...'); } else {
            if (this.valve.expiry_date.toString() === '0001-01-01T00:00:00') { this.alertify.error('Please enter expiry date ...'); } else {
                if (this.valve.serial_no === '') { this.alertify.error('Please enter valve serial number ...'); } else {
                    if (this.valve.size === '') { this.alertify.error('Please enter valve size ...'); } else {
                        help = true;
                    }
                }
            }
        }

        const currentDate = new Date();
        if (moment(currentDate).isAfter(this.valve.expiry_date)) {
            this.alertify.error('This valve is already expired ...');
            help = false;
        }


        return help;
    }

}

