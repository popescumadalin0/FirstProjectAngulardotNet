import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-add-edit-department',
  templateUrl: './add-edit-department.component.html',
  styleUrls: ['./add-edit-department.component.css']
})
export class AddEditDepartmentComponent implements OnInit {

  constructor(private service:SharedService) { }

  @Input() depart:any;
  DepartmentId: any;
  DepartmentName: any;

  @Output() public childEvent=new EventEmitter();

  ngOnInit(): void {
    this.DepartmentId=this.depart.DepartmentId;
    this.DepartmentName=this.depart.DepartmentName;
    
  }

  addDepartment()
  {
    var val ={ DepartmentId:this.DepartmentId,
                DepartmentName: this.DepartmentName};
    this.service.addDepartment(val).subscribe(res=>
      {alert(res.toString());
      });

      this.childEvent.emit("Process completed!");
  }
  
  updateDepartment()
  { 
    var val ={ DepartmentId:this.DepartmentId,
                DepartmentName: this.DepartmentName};
    this.service.updateDepartment(val).subscribe(res=>
    {alert(res.toString());
    });

    this.childEvent.emit("Process completed!");
  }

}
