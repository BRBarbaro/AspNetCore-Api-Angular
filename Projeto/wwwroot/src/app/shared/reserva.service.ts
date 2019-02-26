import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Reserva } from './reserva.model';


@Injectable({
  providedIn: 'root'
})
export class ReservaService {

  formData: Reserva;

  constructor(private http: HttpClient) { }

  getReservas() {
    return this.http.get(environment.apiURL + '/reserva').toPromise();
  }

  getReserva(id:number):any {
    return this.http.get(environment.apiURL + '/reserva/'+id).toPromise();
  }

  postReserva() {
    var body = {
      ...this.formData
    };
    delete body.ReservaId;
    return this.http.post(environment.apiURL + '/reserva', body);
  }

  putReserva(id:number) {
    var body = {
      ...this.formData
    };
    return this.http.put(environment.apiURL + '/reserva/'+id, body);
  }

  deleteReserva(id:number) {
    return this.http.delete(environment.apiURL + '/reserva/'+id);
  }  
}
