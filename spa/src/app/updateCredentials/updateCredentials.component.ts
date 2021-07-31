import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-updateCredentials',
  templateUrl: './updateCredentials.component.html',
  styleUrls: ['./updateCredentials.component.scss']
})
export class UpdateCredentialsComponent implements OnInit {

  message = "";
  newPwd1 = "";
  newPwd2 = "";
  oldPwd = "";

  constructor(private alertify: AlertifyService, private auth: AuthService) { }

  ngOnInit() {
  }

  changePassword() {
    if (this.oldPwd === ""){this.message = "Old password can not be empty";} else {
      if (this.newPwd1 === "" || this.newPwd2 === "") {
        this.message = "New passwords can not be empty";
      } else {
        if (this.newPwd1 !== this.newPwd2) { this.message = "New passwords do not match"; }
        else {
          // do your thing
          let test = {username: "", password: ""};
          test.username = this.auth.decodedToken.unique_name;
          test.password = this.newPwd1;
          this.auth.update(test).subscribe((next)=>{
            this.alertify.message(next.toString());
          }, (error)=>{this.alertify.error(error)})
         
        }
      }
    }
   
   
  }

}
