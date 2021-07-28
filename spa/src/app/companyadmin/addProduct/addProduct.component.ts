import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DropItem } from 'src/app/_models/dropItem';
import { TypeOfValve } from 'src/app/_models/TypeOfValve';
import { DropService } from 'src/app/_services/drop.service';
import { ProductService } from 'src/app/_services/product.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { valveSize } from 'src/app/_models/valveSize';

@Component({
  selector: 'app-addProduct',
  templateUrl: './addProduct.component.html',
  styleUrls: ['./addProduct.component.css']
})
export class AddProductComponent implements OnInit {
  @Input() vc: TypeOfValve;
  @Output() povOut: EventEmitter<number> = new EventEmitter();
  typeOfValve: Array<DropItem> = [];
  implantLocation: Array<DropItem> = [];
  newsize = 0; 
  neweoa = 0.0; 
  showAdd = 0;
 

  constructor(private drop: DropService, private prodService: ProductService, private alertify: AlertifyService) { }

  ngOnInit() {

    this.loadDrops();
  }

  displayAdd() { if (this.showAdd === 1) { return true; } }

  updateProductDetails() {
    if (this.vc.type != "") {
      this.prodService.saveDetails(this.vc).subscribe((next) => {
        this.alertify.message('Product saved ...');
        this.povOut.emit(2);
      });
    } else { this.alertify.warning("The type is required ...") }
  }
  cancel() { 
    // remove the newly created product record
    this.prodService.deleteProduct(this.vc.valveTypeId).subscribe((next)=>{
      this.povOut.emit(1);
    })
     }
  addSize() {
    // open the add window
    this.showAdd = 1;
    this.alertify.message('opening window');
  }
  saveSize(){
    this.showAdd = 0;
    const test: valveSize = {sizeId: 0,size: 0, eoa: 0, ppm: ""};
    test.size = this.newsize;
    test.eoa = this.neweoa;
    if(this.vc.product_size === null){
      this.vc.product_size = [];
      this.vc.product_size.push(test);
    } else{this.vc.product_size.push(test);}
   
   }

  deleteSize(id:number){
   this.prodService.deleteValveSize(this.vc.valveTypeId, id).subscribe((next)=>{
     let index = this.vc.product_size.findIndex(a => a.sizeId === id);
    this.vc.product_size.splice(index,1);
    })
  }

  

  loadDrops() {
    if (localStorage.options_product_type === undefined) {
      this.drop.getValveTypeOptions().subscribe((next) => {
        this.typeOfValve = next;
        localStorage.setItem('options_product_type', JSON.stringify(next));
      });
    } else {
      this.typeOfValve = JSON.parse(localStorage.options_product_type);
    }

    if (localStorage.implant_location === undefined) {
      this.drop.getValveLocationOptions().subscribe((next) => {

        this.implantLocation = next;
        localStorage.setItem('implant_location', JSON.stringify(next));
      });
    } else {

      this.implantLocation = JSON.parse(localStorage.implant_location);
    }

  }







}
