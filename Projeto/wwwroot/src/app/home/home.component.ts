import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SalaService } from 'src/app/shared/sala.service';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: []
})
export class HomeComponent implements OnInit {
  salaList;
  userDetails;

  constructor(private service: SalaService,
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.refreshList();
    this.userService.getUsuario().subscribe(
      res => {
        this.userDetails = res;
      },
      err => {
        console.log(err);
      },
    );
  }

  refreshList() {
    this.service.getSalas().then(res => this.salaList = res );
  }

  openForEdit(Id: number) {
    this.router.navigate(['/sala/' + Id]);
  }

  onDelete(id: number) {
    if (confirm('Deseja deletar a sala?')) {
      this.service.deleteSala(id).subscribe(res => {
        this.refreshList();
        this.toastr.warning("Registro deletado com sucesso", "Salas");
      },
      err => {
        this.toastr.error(err.error.message, "Salas");
      });
    }
  }

  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }
}
