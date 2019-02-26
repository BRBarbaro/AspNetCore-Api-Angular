import { Component, OnInit } from '@angular/core';
import { ReservaService } from 'src/app/shared/reserva.service';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-reservas',
  templateUrl: './reservas.component.html',
  styles: []
})
export class ReservasComponent implements OnInit {

  isValid: boolean = true;
  SalaId;
  
  constructor(private service: ReservaService,
    private toastr: ToastrService,
    private router: Router,
    private currentRoute: ActivatedRoute) { }

  ngOnInit() {
    let Id = this.currentRoute.snapshot.paramMap.get('id');
    this.SalaId = localStorage.SalaId;
    if (Id == null)
      this.resetForm();
    else {
      this.service.getReserva(parseInt(Id)).then(res => {
        this.service.formData = {
          ReservaId : res.reservaId,
          Titulo : res.titulo,
          Inicio : res.inicio,
          Fim : res.fim,
          SalaId : res.salaId,
          UsuarioId : res.usuarioId
        };
        this.SalaId = res.salaId
      });
    }
  } 

  resetForm(form?: NgForm) {
    if (form = null)
      form.resetForm();
    
    this.service.formData = {
      ReservaId : null,
      Titulo : '',
      Inicio : '',
      Fim : '',
      SalaId : this.SalaId,
      UsuarioId : null
    };
  }

  validateForm() {
    this.isValid = true;
    if (!this.service.formData.Titulo || 
        !this.service.formData.Inicio ||
        !this.service.formData.Fim)
      this.isValid = false;

    return this.isValid;
  }

  onSubmit(form: NgForm) {
    if (this.validateForm()) {
      if (this.service.formData.ReservaId)
        this.service.putReserva(this.service.formData.ReservaId).subscribe(res => {
          this.resetForm();
          this.toastr.success('Registro alterado com sucesso', 'Reservas');
          this.router.navigate(['/sala/'+ this.SalaId]);
        }, err => {
          this.toastr.error(err.error.message, 'Reservas');
        })
      else 
        this.service.postReserva().subscribe(res => {
          this.resetForm();
          this.toastr.success('Registro criado com sucesso', 'Reservas');
          this.router.navigate(['/sala/'+ this.SalaId]);
        }, err => {
          this.toastr.error(err.error.message, 'Reservas');
        })
    }
  }
}


