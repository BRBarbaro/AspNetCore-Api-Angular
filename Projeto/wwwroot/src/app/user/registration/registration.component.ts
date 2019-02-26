import { UserService } from './../../shared/user.service';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styles: []
})
export class RegistrationComponent implements OnInit {

  constructor(public service: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.service.formModel.reset();
  }

  onSubmit() {
    this.service.register().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.service.formModel.reset();
          this.toastr.success('Usuario criado com sucesso', 'Usuarios');
        } else {
          this.toastr.error(res.error.message,'Usuarios');
        }
      },
      err => {
        console.log(err)
        this.toastr.error(err.error.message,'Usuarios');
      }
    );
  }

}
