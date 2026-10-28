# System Architecture

## Overview
This system uses a 3-tier architecture:
1. **Presentation Layer**: Console CLI (`Program.cs`)
2. **Business & Data Layer**: Services and ADO.NET Repositories
3. **Database Layer**: SQL Server Express (`TicketDB`)

## Transaction Strategy
`TicketRepository.CreateTransactionalAsync` guarantees atomic operations:
- Insert Ticket record
- Insert initial TicketUpdate entry
- Insert Audit Log entry