import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from '../../environments/environment';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import * as _ from 'underscore';
import { Photo } from '../_models/Photo';


@Component({
  selector: 'app-photo-editor',
  templateUrl: './photoEditor.component.html',
  styleUrls: ['./photoEditor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() userId: number;
  @Input() companyCode: number;
  @Input() valvecode: number;
  @Input() hospitalId: number;
  @Output() getMemberPhotoChange: EventEmitter<string> = new EventEmitter();
  uploader: FileUploader;
  currentMainPhoto: Photo;

  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;

  constructor(private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService) { }

  ngOnInit() { this.initializeUploader(); }

  public fileOverBase(e: any): void { this.hasBaseDropZoneOver = e; }

  initializeUploader() {
    debugger;
    let test = '';
    if (typeof this.userId !== 'undefined' && this.userId !== 0 ) {
      debugger;
      test = this.baseUrl + 'addUserPhoto/' + this.userId
    }
    else {
      if (typeof this.hospitalId !== 'undefined' && this.hospitalId !== 0) {
        test = this.baseUrl + 'hospital/addHospitalPhoto/' + this.hospitalId
      }
      else {
        if (typeof this.valvecode !== 'undefined' && this.valvecode !== 0) {
          test = this.baseUrl + 'addProductTypePhoto/' + this.valvecode
        } else {
          if (typeof this.companyCode !== 'undefined' && this.companyCode !== 0) {
             test = this.baseUrl + 'addCompanyLogo/' + this.companyCode
          }
        }
      }

    }


    this.uploader = new FileUploader({
      url: test,
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: any = JSON.parse(response);
        if (this.hospitalId !== 0) { this.getMemberPhotoChange.emit(res.ImageUrl); }
        if (this.userId !== 0) {this.getMemberPhotoChange.emit(res.photoUrl); }
        if (this.valvecode !== 0) { this.getMemberPhotoChange.emit(res.image); }
        if (this.companyCode !== 0) { this.getMemberPhotoChange.emit(res.reps); }
     }
    };
  }

  
  



}
