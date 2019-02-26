 import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Sala} from './sala.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SalaService {
  formData: Sala;

  constructor(private http: HttpClient) { }

  getSalas() {
    return this.http.get(environment.apiURL + '/sala').toPromise();
  }

  getSala(id:number):any {
    return this.http.get(environment.apiURL + '/sala/'+id).toPromise();
  }

  postSala() {
    var body = {
      ...this.formData
    };
    delete body.Id;
    return this.http.post(environment.apiURL + '/sala', body);
  }

  putSala(id:number) {
    var body = {
      ...this.formData
    };
    return this.http.put(environment.apiURL + '/sala/'+id, body);
  }

  deleteSala(id:number) {
    return this.http.delete(environment.apiURL + '/sala/'+id);
  }

}