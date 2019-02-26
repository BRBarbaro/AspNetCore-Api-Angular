import { Component, OnInit } from '@angular/core';
import { SalaService } from 'src/app/shared/sala.service';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ReservaService } from 'src/app/shared/reserva.service';

@Component({
  selector: 'app-sala',
  templateUrl: './sala.component.html',
  styles: []
})
export class SalaComponent implements OnInit {

  isValid: boolean = true;
  reservaList;
  SalaId;
  
  constructor(private service: SalaService,
    private reservaService: ReservaService,
    private toastr: ToastrService,
    private router: Router,
    private currentRoute: ActivatedRoute) { }

  ngOnInit() {
    this.SalaId = this.currentRoute.snapshot.paramMap.get('id');
    if (this.SalaId == null)
      this.resetForm();
    else {
      this.service.getSala(parseInt(this.SalaId)).then(res => {
        this.service.formData = {
          Id: res.id,
          Nome: res.nome,
          Capacidade: res.capacidade,
          UsuarioId: res.usuarioId
        };
        this.refreshList()
      });
    }
    localStorage.SalaId = this.SalaId;
  } 

  refreshList() {
    this.service.getSala(parseInt(this.SalaId)).then(res => this.reservaList = res.reservas );
  }

  resetForm(form?: NgForm) {
    if (form = null)
      form.resetForm();
    
    this.service.formData = {
      Id: null,
      Nome: '',
      Capacidade: 0,
      UsuarioId: ''
    };
  }

  validateForm() {
    this.isValid = true;
    if (!this.service.formData.Nome || 
        this.service.formData.Capacidade <= 0)
      this.isValid = false;
    return this.isValid;
  }

  onSubmit(form: NgForm) {
    if (this.validateForm()) {
      if (this.service.formData.Id)
        this.service.putSala(this.service.formData.Id).subscribe(res => {
          this.resetForm();
          this.toastr.success('Registro alterado com sucesso', 'Salas');
          this.router.navigate(['/home']);
        }, err => {
          this.toastr.error(err.error.message, 'Salas');
        })
      else 
        this.service.postSala().subscribe(res => {
          this.resetForm();
          this.toastr.success('Registro criado com sucesso', 'Salas');
          this.router.navigate(['/home']);
        }, err => {
          this.toastr.error(err.error.message, 'Salas');
        })
    }
  }

  openForEdit(Id: number) {
    this.router.navigate(['/reservas/' + Id]);
  }

  onDelete(id: number) {
    if (confirm('Deseja deletar a reserva?')) {
      this.reservaService.deleteReserva(id).subscribe(res => {
        this.refreshList();
        this.toastr.warning("Registro deletado com sucesso", "Reservas");
      },
      err => {
        this.toastr.error(err.error.message, "Reservas")
      });
    }
  }
}
