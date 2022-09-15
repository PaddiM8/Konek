# Server

## Core Specification

### Hub

A hub contains an arbitrary amount of lamps and lamp groups.

### Lamp Group

A lamp group contains an arbitrary amount of lamps. A lamp can
be a part of several groups. Routines can be applied to entire
groups in order to be added to all the lamps in that group.
Every lamp group has a priority index which is used to decide
which group's routine should be prioritised in cases where
a lamp is a part of several groups with overlapping routines.

### Lamp

A lamp contains an arbitrary amount of routines. A lamp's
temperature and brightness at a certain point in time is
determined by looking at the state of the active routine.
Routines are ordered by priority.

### Routine

A routine contains a number of effects. The start and end
times of effects in a routine may never overlap. Every
routine spans 24 hours but may have periods with no
specified effects. In these cases, a routine of lesser
priority is queried instead.

It is only possible to remove a routine definition if there
are no routine instances of it.

### Effect

An effect contains a start time, end time, start temperature
end temperature, start brightness, and end brightness. The
start and end times are represented HH:mm.