import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-add-edit-employee',
  templateUrl: './add-edit-employee.component.html',
  styleUrls: ['./add-edit-employee.component.css']
})
export class AddEditEmployeeComponent implements OnInit {

  constructor(private service:SharedService) { }

  @Input() empl:any;
  EmployeeId: any;
  EmployeeName: any;
  Department: any;
  DateOfJoining: any;
  PhotoFileName: any;
  PhotoFilePath: any;
  
  @Output() public childEvent=new EventEmitter();

  DepartmentList:any=[];

  ngOnInit(): void {
    this.loadDepartmentList();
  }

  loadDepartmentList(){
    this.service.getAllDepartmentNames().subscribe((data:any)=>{
        this.DepartmentList=data;

      this.EmployeeId=this.empl.EmployeeId;
      this.EmployeeName=this.empl.EmployeeName;
      this.Department=this.empl.Department;
      this.DateOfJoining=this.empl.DateOfJoining;
      this.PhotoFileName=this.empl.PhotoFileName;
      this.PhotoFilePath=this.service.PhotoUrl+this.PhotoFileName;
    });
  }

  addEmployee()
  {
    var val ={ EmployeeId:this.EmployeeId,
                EmployeeName: this.EmployeeName,
                Department: this.Department,
                DateOfJoining: this.DateOfJoining,
                PhotoFileName: this.PhotoFileName};
    this.service.addEmployee(val).subscribe(res=>
      {alert(res.toString());
      });

      this.childEvent.emit("Is working!");
    
  }
  
  updateEmployee()
  { 
    var val ={ EmployeeId:this.EmployeeId,
      EmployeeName: this.EmployeeName,
      Department: this.Department,
      DateOfJoining: this.DateOfJoining,
      PhotoFileName: this.PhotoFileName};
    this.service.updateEmployee(val).subscribe(res=>
    {alert(res.toString());
    });

    this.childEvent.emit("Is working!");
  }

  uploadPhoto(event:any)
  {
    var file=event.target.files[0];
    const formData: FormData= new FormData();
    formData.append('uploaded',file,file.name);

    this.service.UploadPhotos(formData).subscribe((data:any)=>{
      this.PhotoFileName=data.toString();
      this.PhotoFilePath=this.service.PhotoUrl+this.PhotoFileName;
    });
  }

}
