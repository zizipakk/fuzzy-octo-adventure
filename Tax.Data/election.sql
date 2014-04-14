select k.Firstname, k.LastName, s.SinoszId, u.Email, po.Code, s.HomeAddress, o.OrganizationName, poo.Country, k.isCommunicationRequested, k.isDeviceReqested, p.IsNeedForHealth, p.IsNeedForJob, p.IsNeedForLife
from dbo.AspNetUsers u
join dbo.SinoszUsers s on u.SinoszUser_Id = s.Id
left outer join Postcodes po on s.Postcode_Id = po.Id
join dbo.Organizations o on s.Organization_Id = o.Id
left outer join dbo.Postcodes poo on o.Postcode_Id = poo.Id
join dbo.KontaktUsers k on u.KontaktUser_Id = k.Id
left outer join dbo.PreregistrationDatas p on k.PreregistrationData_Id = p.Id --52 elveszett az elején
where u.isEmailValidated = 1
