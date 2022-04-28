import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-show-employee',
  templateUrl: './show-employee.component.html',
  styleUrls: ['./show-employee.component.css']
})
export class ShowEmployeeComponent implements OnInit {

  constructor(private service: SharedService) {}

  EmployeeList: any = [];

  ModalTitle: string = "";
  ActivateAddEditEmployee: boolean = false;
  emp: any;
  wasAdded:boolean=false;

  ngOnInit(): void {
    this.refreshEmployeeList();
    
  }

  addClick() {
    this.emp = {
      EmployeeId: 0,
      EmployeeName: "",
      empartment: "",
      DateOfJoining: "",
      PhotoFileName:"anonymous.png"
    };

    this.ModalTitle = "Add Employee";
    this.ActivateAddEditEmployee = true;
    this.wasAdded=false;
  }

  editClick(item: any) {
    this.emp = item;
    this.ModalTitle = "Edit Employee";
    this.ActivateAddEditEmployee = true;
    this.wasAdded=false;
  }

  deleteClick(item: any) {
    if (confirm("Are you sure?")) {
      this.service.deleteEmployee(item.EmployeeId).subscribe(data => {
        this.refreshEmployeeList();
        alert(data.toString());
      });
    }
  }

  closeClick() {
    this.ActivateAddEditEmployee = false;
    this.refreshEmployeeList();
    this.wasAdded=true;
  }

  refreshEmployeeList() {
    this.service.getEmployeeList().subscribe(data => {
      this.EmployeeList = data;
    });
  }

}
