import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  readonly APIUrl="https://localhost:7151/api";
  readonly PhotoUrl="http://localhost:7151/Photos/";


  constructor(private http:HttpClient) { }

  getDepartmentList(): Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/Department');
  }

  addDepartment(val:any)
  {
    return this.http.post(this.APIUrl+'/Department',val);
  }
  updateDepartment(val:any)
  {
    return this.http.put(this.APIUrl+'/Department',val);
  }
  deleteDepartment(val:any)
  {
    return this.http.delete(this.APIUrl+'/Department/'+val);
  }

  getEmployeeList(): Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/Employee');
  }

  addEmployee(val:any)
  {
    return this.http.post(this.APIUrl+'/Employee',val);
  }
  updateEmployee(val:any)
  {
    return this.http.put(this.APIUrl+'/Employee',val);
  }
  deleteEmployee(val:any)
  {
    return this.http.delete(this.APIUrl+'/Employee/'+val);
  }

  UploadPhotos(val:any)
  {
    return this.http.post(this.APIUrl +'/Employee/SaveFile',val);
  }

  getAllDepartmentNames():Observable<any[]>
  {
    return this.http.get<any[]>(this.APIUrl+'/Employee/GetAllDepartmentNames');
  }

}
