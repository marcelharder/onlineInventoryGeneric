import { Component, Input, OnInit } from '@angular/core';
import { Location } from 'src/app/_models/Location';

@Component({
  selector: 'app-addLocation',
  templateUrl: './addLocation.component.html',
  styleUrls: ['./addLocation.component.scss']
})
export class AddLocationComponent implements OnInit {
  @Input() loc: Location;

  constructor() { }

  ngOnInit() {
  }

}
